import { observable } from "mobx";
import { observer } from "mobx-react-lite";
import Button from "../../Controls/Button/Button";
import InputPassword from "../../Controls/Input/InputPassword";
import {Api} from "../../Core/api"

import "../../App.css";
import loginStore from "../../Core/LoginStore";
import Strings from "../../Core/LocalizableStrings";
import ISetting from "../../Core/ISetting";
import { useHistory } from "react-router-dom";
import settings from "./data/Settings";

import styles from "./SettingPage.module.css";
import HeaderPage from "../../Controls/HeaderPage/HeaderPage";



function SettingPage(){
    const items:ISetting[] = settings;
     return(
        <>
            <HeaderPage title="Настройки">
                
            </HeaderPage>
            <div className="gap row">
                {items.map(i => <SettingItem {...i} />)}
            </div>
        </>
        
    )
}

function SettingItem(props:ISetting){
    const history = useHistory();

    function onClick(){
        history.push("settings/"+props.url);
    }
    
    return(
        <div className={`column content ${styles.body}`} onClick={onClick}>
            {props.icon}
            {props.caption}
        </div>
    )
}

export default observer(SettingPage);