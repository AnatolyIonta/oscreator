import { useEffect, useState } from 'react';
import {
  Switch,
  Route,
  Redirect,
  BrowserRouter, Link, useLocation
} from "react-router-dom";

import './App.css';

import LoadAssemblyPage from "./Pages/ModuleLoader/ModuleLoader"
import LoginPage from './Pages/LoginPage/LoginPage';
import loginStore from './Core/LoginStore';
import { observer } from 'mobx-react-lite';
import SettingPage from './Pages/SettingPage/SettingPage';
import AboutPage from './Pages/AboutPage/AboutPage';
import ModulePage from './Pages/ModulePage/ModulePage';
import { setAppDomen } from './Core/Configure';
import LoggerPage from './Pages/LoggerPage/Page/LoggerPage';
import UsersSetting from './Pages/SettingPage/settings/users/usersSetting';
import CustomPage from './Pages/CustomPage/CustomPage';
import Header from './Controls/Header/Header';
import NavBar from './Controls/NavBar/NavBar';
import workspaceStore from './Core/WorkspaceStore';

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
      <Header user='Admin'/>
      <div className='body'>
        <NavBar/>
        <div className='content'>
          <Switch>
            <Route path="/" exact>
              <Redirect exact from="/" to="/additions"/>
            </Route>
            <Route path="/additions">
              <LoadAssemblyPage/>
            </Route>
            <Route path="/settings" exact={true}>
              <SettingPage/>
            </Route>
            <Route path="/settings/users" exact={true}>
              <UsersSetting/>
            </Route>
            <Route path="/about">
              <AboutPage/>
            </Route>
            <Route path='/module/:id'>
              <ModulePage/>
            </Route>
            <Route path='/logs'>
              <LoggerPage/>
            </Route>
            <Route path="/:url">
              <CustomPage/>
            </Route>
          </Switch>
        </div>

      </div>
    </div>
  )
}

export default App;