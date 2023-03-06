import { useState } from "react";
import Input from "../../Controls/Input/Input";
import InputPassword from "../../Controls/Input/InputPassword";

import "../../App.css";
import styles from './LoginPage.module.css';
import Button from "../../Controls/Button/Button";
import { Api } from "../../Core/api";
import loginStore from "../../Core/LoginStore";
import { observer } from "mobx-react-lite";
import Strings from "../../Core/LocalizableStrings";

function LoginPage(){
    const [name,setName] = useState<string>("");
    const [password, setPassword] = useState<string>("");

    async function login(){
        const data = {
            login:name,
            password: password
        }
        var response = await Api.postAuth("user/login", data);
        if(response.status == 200){
            var json = await response.json();
            loginStore.logIn(json.jwt);
        }
    }

    return(
        <div className={styles.wrapper}>
            <div className={`column ${styles.content}`}>
                <h2 style={{margin:"0"}}>{Strings.LoginPage.entrance}</h2>
                <label htmlFor="name">
                    <span>{Strings.LoginPage.login}</span>
                    <br/>
                    <Input name={"name"} id={"name"} onChangeValue={(e)=>setName(e)} value={name} />
                </label>
                <label htmlFor="password">
                    <span>{Strings.LoginPage.password}</span>
                    <br/>
                    <InputPassword name={"password"} id={"password"} onChangeValue={(e)=>setPassword(e)} value={password} />
                </label>
                <Button title="Войти" onClick={login}/>
            </div>
        </div>
    )
}

export default observer(LoginPage);