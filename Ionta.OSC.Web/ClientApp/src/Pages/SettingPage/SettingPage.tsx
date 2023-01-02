import { observable } from "mobx";
import React from "react";
import Button from "../../Controls/Button/Button";
import InputPassword from "../../Controls/Input/InputPassword";

const loginData = observable({
    password:"",
    passwordReturn:"",
    valid:false,
})

function SettingPage(){
     const f = loginData;
    return(
        <div>
            <h2>Сменить пароль</h2>
            <InputPassword onChangeValue={e => f.password = e} value={f.password}/>
            <InputPassword onChangeValue={e => f.passwordReturn = e} value={f.passwordReturn}/>
            <Button onClick={} title={"сменить пароль"}/>
        </div>
    )
}