import React, { useEffect, useRef } from "react";
import Button, { ButtonFileLoad, ButtonStyles } from "../../Controls/Button/Button";
import Strings from "../../Core/LocalizableStrings";
import store from "./ModuleLoaderStore";

import styles from "./ModuleLoader.module.css";
import '../../App.css';

import { observer } from "mobx-react-lite";
import { ApiDomen } from "../../Core/Configure";
import { observable } from "mobx";
import { Api } from "../../Core/api";
import loginStore from "../../Core/LoginStore";



function LoadAssemblyPage(){
    useEffect(()=>{
        store.load();
    },[]);

    return(
        <div>
            <LoadAssemblyToolBar/>
            <LoadAssemblyList data={store.data}/>
        </div>
    )
}

function LoadAssemblyToolBar(){
    const dialogRef = useRef<HTMLDialogElement>(null);

    function onChangeHandler(e: React.ChangeEvent<HTMLInputElement>) {
        const file = e.target!.files![0];
        let data = new FormData()
        data.append('file', file);
        fetch(ApiDomen+'/Assembly/SaveAssembly', {
            headers: {
                Authorization: "Bearer " + loginStore.token,
            },
            method: 'POST',
            body: data
        }).then(e => store.load());
    }


    return(
        <>
            <div className='row justBetween alignCenter' style={{marginBottom:"30px"}}>
                <h3>Загрузчик модулей</h3>
                <ButtonFileLoad title={Strings.AsemblyPage.loadButton} onChange={onChangeHandler}/>
            </div>
            <dialog ref={dialogRef}>
                <h1>Опа! Здарова!</h1>
            </dialog>
        </>
    )
}
  
function LoadAssemblyList(props:{data:any[] | null}){
    return(
      <div className='column gap'>
        {props.data?.map((e:any, key) => <LoadAssemblyListItem key={key} name={e.name}  id={e.id} isActive={e.isActive} />)}
      </div>
    )
}
  
function LoadAssemblyListItem(props:{name:string, id:number, isActive:boolean}){
    let className = `${styles.block} row`;
    if(!props.isActive){
        className = className + " " + styles.blockOff
    }

    let classNameDicorateBox = `${styles.dicorateBox}`;
    if(!props.isActive){
        classNameDicorateBox = classNameDicorateBox + " " + styles.dicorateBoxOff;
    }

    function enableModul(){
        const data = {assemblyId: props.id, isActive: !props.isActive};
        let response = Api.postAuth("Assembly/SetActive", data).then(e => { Api.postAuth("Assembly/ApplayMigration",{}); store.load() });
    }

    const dialogRef = useRef<HTMLDialogElement>( null );

    function deleteModulClick(){
        dialogRef.current?.showModal();
    }

    function deleteModulClose(){
        dialogRef.current?.close();
    }

    function deleteModul(){
        const data = {assemblyId: props.id, isActive: false};
        let response = Api.postAuth("Assembly/SetActive", data)
        .then(e => {
            const data = {id: props.id};
            let response2 = Api.postAuth("Assembly/delete", data)
            .then(c => {store.load(); deleteModulClose()})
        });
    }
    
    return(
        <>
            <div className={className}>
                <div className='row margin-right alignCenter'>
                <div className={classNameDicorateBox}/>
                <span className={styles.nameModul}>{props.name}</span>
                </div>
                
                <div className='row margin-right alignCenter justCenter'>
                <Button onClick={enableModul} title={props.isActive ? Strings.AsemblyPage.disableModule : Strings.AsemblyPage.enambleModule}/>
                <Button onClick={deleteModulClick} title={Strings.AsemblyPage.deleteModule} buttonStyle={ButtonStyles.red}/>
                </div>
            </div>
            <dialog ref={dialogRef}>
                <span>Вы уверены, что хотите удалить модуль?</span>
                <menu className="row margin-right alignCenter justCenter">
                    <Button title="Отмена" onClick={deleteModulClose} />
                    <Button title="Потвердить" onClick={deleteModul} />
                </menu>
            </dialog>
      </>
    );
}

export default observer(LoadAssemblyPage);