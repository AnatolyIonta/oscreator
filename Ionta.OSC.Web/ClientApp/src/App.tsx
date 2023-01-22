import React, { useEffect, useRef, useState } from 'react';
import {
  Switch,
  Route,
  Redirect,
  useLocation, useHistory, BrowserRouter, Link
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
import SettingPage from './Pages/SettingPage/SettingPage';
import AboutPage from './Pages/AboutPage/AboutPage';
import { setAppDomen } from './Core/Configure';


function App() {
    const [loading, setLoading] = useState<boolean>(false);
    useEffect(() => {
        async function fun() {
            await setAppDomen();
            setLoading(true);
        }
        fun();
    }, [])
    return (
        <>
            {loading ? < Router /> : <div>Loading</div>}
      </>
   
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
            <BrowserRouter>
              <AdminPanel/>
            </BrowserRouter>
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
            <Link to={"/additions"}><span>{Strings.Nav.Libary}</span></Link>
          </div>
          <div className='navItem'>  
            <Setting fill='#e4e9ed' stroke='#e4e9ed'/>
            <Link to={"/settings"}><span>{Strings.Nav.Setting}</span></Link>
          </div>
          <div className='navItem'>   
            <Info fill='#e4e9ed' stroke='#e4e9ed'/>
            <Link to={"/about"}><span>{Strings.Nav.About}</span></Link>
          </div>
        </div>

        <div className='content'>
          <Switch>
            <Route path="/" exact>
              <Redirect exact from="/" to="/additions"/>
            </Route>
            <Route path="/additions">
              <LoadAssemblyPage/>
            </Route>
            <Route path="/settings">
              <SettingPage/>
            </Route>
            <Route path="/about">
              <AboutPage/>
            </Route>
          </Switch>
        </div>

      </div>
    </div>
  )
}

export default App;
