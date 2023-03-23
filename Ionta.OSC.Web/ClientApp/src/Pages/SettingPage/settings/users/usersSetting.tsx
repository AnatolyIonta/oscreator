import "../../../../App.css";

import Button from "../../../../Controls/Button/Button";
import InputPassword from "../../../../Controls/Input/InputPassword";
import Strings from "../../../../Core/LocalizableStrings";
import loginStore from "../../../../Core/LoginStore";
import { observable } from "mobx";
import { Api } from "../../../../Core/api";

interface ILoginData{
    password: string,
    passwordReturn: string,
    oldPassword: string,
    valid: ()=> boolean,
    secses: boolean | null,
}

const loginData : ILoginData = observable<ILoginData>({
    password: "",
    passwordReturn: "",
    oldPassword: "",
    valid: () => loginData.password == loginData.passwordReturn,
    secses: null,
})

function UsersSetting(){
    const f = loginData;

    async function onClick(){
        if(f.valid()){
            let data = {
                password: f.password,
                oldPassword: f.oldPassword
            }
            var response = await Api.postAuth("user/сhangepassword", data);
            f.secses = response.status == 200;
        }
    }

    return (
        <div className="gap column">
            <h2>{Strings.SettingPage.description}</h2>
            <span className="errorMessage" hidden={f.secses || f.secses === null}>{Strings.SettingPage.errorExternal}</span>
            <span className="successMessage" hidden={!f.secses}>{Strings.SettingPage.success}</span>
            <span className="errorMessage" hidden={f.valid()}>{Strings.SettingPage.errorInternal}</span>
            <label>
                <span>{Strings.SettingPage.newPassword}</span>
                <br/>
                <InputPassword onChangeValue={e => f.password = e} value={f.password}/>
            </label>
            <label>
                <span>{Strings.SettingPage.passwordRepeat}</span>
                <br/>
                <InputPassword onChangeValue={e => f.passwordReturn = e} value={f.passwordReturn}/>
            </label>
            <label>
                <span>{Strings.SettingPage.oldPassword}</span>
                <br/>
                <InputPassword onChangeValue={e => f.oldPassword = e} value={f.oldPassword}/>
            </label>
            <Button onClick={onClick} title={Strings.SettingPage.changePassword}/>
            <Button onClick={()=>loginStore.logOut()} title={Strings.SettingPage.logOut}/>
        </div>
    );
}

export default UsersSetting;