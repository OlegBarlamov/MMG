import React from 'react';
import './App.css';
import CanvasViewComponent from "./canvas/CanvasViewComponent";
import {DefaultCanvasService} from "./canvas/DefaultCanvasService";
import {WsWidgetsService} from "./models/IWidgetsService";
import {WebSocketServiceImpl} from "./ws-api/IWebSocketService";

function App() {
    initializeApplication()
    
    const wsService = new WebSocketServiceImpl() 
    const widgetsService = new WsWidgetsService(wsService)
    const canvasService = new DefaultCanvasService(widgetsService)
    
    wsService.connect('ws://localhost:8999')
    
    return (
        <div className="App">
            <CanvasViewComponent canvasService={canvasService} widgetsService={widgetsService} />
        </div>
    );
}

function initializeApplication() {
    
}

export default App;
