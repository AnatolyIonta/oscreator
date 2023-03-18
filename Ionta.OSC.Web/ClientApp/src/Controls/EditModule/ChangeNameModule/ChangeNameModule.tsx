import { useState } from "react";
import { Api } from "../../../Core/api"; 

import Button from "../../Button/Button"; 
import Input, {InputStyles} from "../../Input/Input";
import Strings from "../../../Core/LocalizableStrings"; 

import {ReactComponent as Done} from "../../../Icon/done.svg"
import {ReactComponent as Cancel} from "../../../Icon/cancel.svg"
import styles from './ChangeNameModule.module.css';
import pageStore from "../../../Pages/ModulePage/ModulePageStore";

export default function ChangeNameModule(props: {id: string}) {
    const [name, setName] = useState<string>("");
    const [nameSaved, setSave] = useState<boolean>(false);
    const [error, setError] = useState<boolean>(false);

    async function sendName() {
        if(name === '') {
            return setError(true), setSave(false);
        }

        const data = {
            name: name,
            id: props.id
        }
        
        var res = await Api.postAuth("assembly/changeName", data);
        if (res.ok) {
            setSave(await res.json());
            setError(false);
            pageStore.load();
        } 
    }

    return(
        <div className='row justCenter'>
            <div className={styles.container}>
                <span>{Strings.ModulePage.changeNameModule}</span>
                <div>
                    <div className={styles.input}>
                        <Input name={"name"} id={"name"} onChangeValue={(e)=>setName(e)} value={name} inputStyles={InputStyles.classic}/>
                    </div>
                    <OperationSucces nameSaved={nameSaved}/>
                    <OperationError error={error}/>
                </div>
                <div className='row justCenter widthFull'>
                    <Button title={Strings.ModulePage.save} onClick={sendName}/>
                </div>
            </div>
        </div>
    )
}

function OperationSucces(props:{nameSaved: boolean}) {
    return(
        <div>{props.nameSaved === false ? <></> : 
            <>
                <div className={styles.textSucces}>
                    <Done fill='#7ad663' stroke='#7ad663'/>
                    <span className='row wrap alignCenter'>{Strings.ModulePage.savedSucces}</span>
                </div>
            </>}
        </div>
    )
}

function OperationError(props:{error: boolean}) {
    return(
        <div>{props.error === false ? <></> : 
            <>
                <div className={styles.textError}>
                    <Cancel fill='#d66363' stroke='#d66363'/>
                    <span className='row wrap alignCenter'>{Strings.ModulePage.modulNameCantBeEmpty}</span>
                </div>
            </>}
        </div>
    )
}