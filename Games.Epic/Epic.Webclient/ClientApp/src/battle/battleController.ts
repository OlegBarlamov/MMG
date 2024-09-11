import {IBattleMapController} from "../battleMap/battleMapController";
import {BattleMapUnit} from "../battleMap/battleMapUnit";
import {BattleMap, BattleMapCell} from "../battleMap/battleMap";
import {BattleUserAction} from "./battleUserAction";
import {wait} from "../common/wait";

export interface IBattleController {
    startBattle(): Promise<void>
}

export class BattleController implements IBattleController {
    mapController: IBattleMapController

    private readonly map: BattleMap
    private readonly orderedUnits: BattleMapUnit[]
    
    private battleStarted: boolean = false
    private battleFinished: boolean = false
    private currentStepUnitIndex: number = -1
    
    constructor(mapController: IBattleMapController) {
        this.mapController = mapController

        this.orderedUnits = [...this.mapController.map.units]
            .sort((a, b) => b.props.speed - a.props.speed)
        this.map = mapController.map
    }

    async startBattle(): Promise<void> {
        if (this.battleStarted) throw new Error("The battle already started");
        this.battleStarted = true
        this.currentStepUnitIndex = -1
        
        while (!this.battleFinished) {
            this.currentStepUnitIndex++
            if (this.currentStepUnitIndex >= this.orderedUnits.length) {
                this.currentStepUnitIndex = 0
            }
            
            const currentUnit = this.orderedUnits[this.currentStepUnitIndex]
            await this.processStep(currentUnit)
        }
    }

    private async processStep(unit: BattleMapUnit): Promise<void> {
        await this.mapController.activateUnit(unit)
        
        const cellsForMove = this.getCellsForUnitMove(unit)
        const movableCellsColor = 0x6dbfac
        cellsForMove.forEach(cell => this.mapController.setCellDefaultColor(cell.r, cell.c, movableCellsColor))

        this.mapController.onCellMouseEnter = (cell) => {
            if (cellsForMove.indexOf(cell) >= 0) {
                this.mapController.setCellCustomColor(cell.r, cell.c, 0x32a852)
            }
        }
        this.mapController.onCellMouseLeave = (cell) => {
            if (cellsForMove.indexOf(cell) >= 0) {
                this.mapController.setCellCustomColor(cell.r, cell.c, undefined)
            }
        }
        
        let action: BattleUserAction
        try {
            action = await this.getUserInputAction(unit, cellsForMove)
        } finally {
            this.mapController.onCellMouseEnter = null
            this.mapController.onCellMouseLeave = null
            
            cellsForMove.forEach(cell => {
                this.mapController.restoreCellDefaultColor(cell.r, cell.c)
                this.mapController.setCellCustomColor(cell.r, cell.c)
            })

            await this.mapController.deactivateUnit()
        }
        
        await this.processInputAction(action)
        
        await wait(500)
        
        debugger
    }
    
    private processInputAction(action: BattleUserAction): Promise<void> {
        if (action.targetCell) {
            return this.mapController.moveUnit(action.unit, action.targetCell.r, action.targetCell.c)
        }
        
        throw new Error("Unknown type of user action")
    }
    
    private getUserInputAction(originalUnit: BattleMapUnit, cellsToMove: BattleMapCell[]): Promise<BattleUserAction> {
        return new Promise((resolve) => {
            this.mapController.onCellMouseUp = (cell) => {
                if (cellsToMove.indexOf(cell) >= 0) {
                    this.mapController.onCellMouseUp = null
                    resolve({
                        unit: originalUnit,
                        targetCell: cell,
                    })
                }
            }
        })
    }
    
    private getCellsForUnitMove(unit: BattleMapUnit): BattleMapCell[] {
        const start = unit.position
        const moveRange = unit.props.speed
        const availableCells: BattleMapCell[] = []

        // Use a set to track visited cells
        const visited: Set<string> = new Set()

        // BFS queue for cells to explore; each entry is [cell, distance]
        const queue: [BattleMapCell, number][] = [[start, 0]]

        // Convert a cell to a unique string representation
        const cellToString = (cell: BattleMapCell) => `${cell.r},${cell.c}`
        
        const isCellBlocked = (cell: BattleMapCell) => {
            return this.mapController.getUnit(cell.r, cell.c)
        }

        // Start BFS
        while (queue.length > 0) {
            const [currentCell, distance] = queue.shift()!
            const key = cellToString(currentCell)
            
            // Skip if we've already visited this cell, or it exceeds movement range
            if (visited.has(key) || distance > moveRange) continue

            // Mark this cell as visited
            visited.add(key)

            // If it's not blocked by another unit and within bounds, add to available cells
            if (isCellBlocked(currentCell) && currentCell !== start) {
                continue
            }

            if (currentCell !== start) {
                availableCells.push(currentCell)
            }

            // Get neighboring cells and continue exploring
            const neighbors = this.mapController.map.grid.getNeighborCells(currentCell.r, currentCell.c)
            for (const neighbor of neighbors) {
                if (!visited.has(cellToString(neighbor))) {
                    queue.push([neighbor, distance + 1]);
                }
            }
        }

        return availableCells;
    }
}