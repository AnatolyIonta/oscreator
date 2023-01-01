import React, { useRef, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import {ReactComponent as Library} from "./Icon/library.svg";
import {ReactComponent as Setting} from "./Icon/settings.svg"
import {ReactComponent as Info} from "./Icon/info.svg"
import {ReactComponent as CheckBox} from "./Icon/check_box.svg"
import Strings from './Core/LocalizableStrings';
import Button, { ButtonStyles } from './Controls/Button/Button';
import {ButtonFileLoad} from './Controls/Button/Button';

function App() {
  return (
    <div className="App">
      <div className='body'>

        <div className='nav'>
          <div className='header'>
            <h2>OSCreator</h2>
          </div>
          <div className='navItem'>  
            <Library fill='#e4e9ed' stroke='#e4e9ed'/>
            <span>{Strings.Nav.Libary}</span>
          </div>
          <div className='navItem'>  
            <Setting fill='#e4e9ed' stroke='#e4e9ed'/>
            <span>{Strings.Nav.Setting}</span>
          </div>
          <div className='navItem'>  
            <Info fill='#e4e9ed' stroke='#e4e9ed'/>
            <span>{Strings.Nav.About}</span>
          </div>
        </div>

        <div className='content'>
          <LoadAssemblyToolBar/>
          <LoadAssemblyList/>
        </div>

      </div>
    </div>
  );
}

function LoadAssemblyToolBar(){
  const dialogRef = useRef<HTMLDialogElement>(null);
  function openDialog(){
    
  }
  return(
    <>
    <div className='row justBetween alignCenter' style={{marginBottom:"30px"}}>
      <h3>Загрузчик модулей</h3>
      <ButtonFileLoad title={Strings.AsemblyPage.loadButton}/>
    </div>
    <dialog ref={dialogRef}>
      <h1>Опа! Здарова!</h1>
    </dialog>
    </>
    
  )
}

function LoadAssemblyList(){
  return(
    <div className='column gap'>
      <LoadAssemblyListItem/>
      <LoadAssemblyListItem/>
      <LoadAssemblyListItem/>
      <LoadAssemblyListItem/>
      <LoadAssemblyListItem/>
      <LoadAssemblyListItem/>
      <LoadAssemblyListItem/>
      <LoadAssemblyListItem/>
      <LoadAssemblyListItem/>
    </div>
  )
}

function LoadAssemblyListItem(){
  return(
    <div className='block row'>
      <div className='row margin-right alignCenter'>
        <div className='dicorateBox'/>
        <span className='nameModul'>Название модуля</span>
      </div>
        
      <div className='row margin-right alignCenter justCenter'>
        <Button onClick={()=>null} title={Strings.AsemblyPage.disableModule}/>
        <Button onClick={()=>null} title={Strings.AsemblyPage.deleteModule} buttonStyle={ButtonStyles.red}/>
      </div>
    </div>
  );
}

export default App;
