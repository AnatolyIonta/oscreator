import React from 'react';
import logo from './logo.svg';
import './App.css';
import {ReactComponent as Library} from "./Icon/library.svg";
import {ReactComponent as Setting} from "./Icon/settings.svg"
import Strings from './Core/LocalizableStrings';

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
            <span>{Strings.Nav.Setting}</span>
          </div>
          <div className='navItem'>  
            <Setting fill='#e4e9ed' stroke='#e4e9ed'/>
            <span>{Strings.Nav.Setting}</span>
          </div>
        </div>

        <div className='content'>
          <div className='block'>
            <span>Выберете файл в формате .dll</span>
            <br/>
            <div>
              <input type="file" name="f" id="f" onChange={()=>{}} value={""}/>
              <button type="submit" id="btn"> Загрузить </button>
            </div>
          </div>
        </div>

      </div>
    </div>
  );
}

export default App;
