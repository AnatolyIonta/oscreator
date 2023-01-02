import React, { useRef, useState } from 'react';
import {
  Switch,
  Route,
  Redirect,
  useLocation, useHistory, BrowserRouter
} from "react-router-dom";


import logo from './logo.svg';
import './App.css';
import {ReactComponent as Library} from "./Icon/library.svg";
import {ReactComponent as Setting} from "./Icon/settings.svg"
import {ReactComponent as Info} from "./Icon/info.svg"
import {ReactComponent as CheckBox} from "./Icon/check_box.svg"
import Strings from './Core/LocalizableStrings';
import Button, { ButtonStyles } from './Controls/Button/Button';
import {ButtonFileLoad} from './Controls/Button/Button';

import LoadAssemblyPage from "./Pages/ModuleLoader/ModuleLoader"
import LoginPage from './Pages/LoginPage/LoginPage';
import loginStore from './Core/LoginStore';
import { observer } from 'mobx-react-lite';


function App() {
  return(
    <Router/>
  )
}

const Router = observer(function (){
  return (
    <BrowserRouter>
        <Switch>
          <Route path="/login">
            {!loginStore.loggedIn ? 
              <LoginPage/>
              : <Redirect exact from="/login" to="/"/>
            }
          </Route>
          <Route path="/">
          {loginStore.loggedIn ? 
            <AdminPanel/>
            : <Redirect exact from="/" to="/login"/>
          }
          </Route>
        </Switch>
    </BrowserRouter>
  );
});

function AdminPanel(){
  return(
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
          <LoadAssemblyPage/>
        </div>

      </div>
    </div>
  )
}

export default App;
