
import Strings from '../../Core/LocalizableStrings';
import { Link, useLocation } from "react-router-dom";
import INavItem from "../../Core/NavItem";

import "../../App.css";
import { observer } from "mobx-react-lite";
import workspaceStore from '../../Core/WorkspaceStore';


interface INavBarProps{
    items:INavItem[];
}

function NavBar(){
    const { pathname } = useLocation();
    const items = workspaceStore.pages;
    
    return(
        <div className='nav'>       
            <div className='header'>
                <h2>OSCreator</h2>
            </div>
            {
                items.map(item => <NavItem key={item.url} item={item} currentPath={pathname}/>)
            }
        </div>
    )
}

function NavItem(props:{item:INavItem, currentPath:string}){
    const item = props.item;
    
    return(
        <div className='navItem'>
            {item.icon}
            <div className={props.currentPath.startsWith(item.url) ? "navItemAactive" : ""}>
              <Link to={item.url}><span>{item.name}</span></Link>
            </div>
        </div>
    )
}

export default observer(NavBar);