import React, { useEffect, useRef, useState } from "react";
import Button, { ButtonFileLoad, ButtonStyles } from "../../Controls/Button/Button";
import Strings from "../../Core/LocalizableStrings";
import store from "./ModuleLoaderStore";

import styles from "./ModuleLoader.module.css";
import '../../App.css';

import { observer } from "mobx-react-lite";
import { ApiDomen } from "../../Core/Configure";
import { Api } from "../../Core/api";
import loginStore from "../../Core/LoginStore";
import Modal from "../../Controls/Modal/Modal";
import { InfoModule } from "./ModalInfoModule/ModalInfoModule";

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
    const [isOpen, setOpen] = useState<boolean>(false);

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
        }).then(_ => store.load());
    }
    
    function ModulOpen(){
        setOpen(true);
    }
    
    function ModulClose(){
        setOpen(false);
    }

    return(
        <>
            <div className='row justBetween alignCenter' style={{marginBottom:"30px"}}>
                <h3>{Strings.AsemblyPage.moduleLoader}</h3>
                <div className="row gap">
                    <Button title={Strings.AsemblyPage.showInformationModule} onClick={ModulOpen}/>
                    <ButtonFileLoad title={Strings.AsemblyPage.loadButton} onChange={onChangeHandler}/>
                </div>
            </div>
            <InfoModule isOpen={isOpen} onClose={ModulClose}/>
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

    const [isOpen, setOpen] = useState<boolean>(false)
    function deleteModulClick(){
        setOpen(true);
    }
    function deleteModulClose(){
        setOpen(false);
    }

    function deleteModul(){
        const data = {assemblyId: props.id, isActive: false};
        let response = Api.postAuth("Assembly/SetActive", data)
        .then(e => {
            const data = {id: props.id};
            let response2 = Api.postAuth("Assembly/delete", data)
            .then(c => {
                store.load(); 
                deleteModulClose();
            })
        });
    }
    function enableModul(){
        const data = {assemblyId: props.id, isActive: !props.isActive};
        let response = Api.postAuth("Assembly/SetActive", data)
        .then(e => { 
            Api.postAuth("Assembly/ApplayMigration",{}); 
            store.load();
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
            <DeleteWarnning isOpen={isOpen} onDeleteModul={deleteModul} onDeleteModulClose={deleteModulClose}/>
      </>
    );
}

interface IDeleteWarnningProps
{
    isOpen: boolean;
    onDeleteModul: () => void;
    onDeleteModulClose: () => void;
}

function DeleteWarnning(props:IDeleteWarnningProps){
    return (
        <Modal isOpen={props.isOpen}>
            <span>{Strings.AsemblyPage.deleteWarnning}</span>
            <menu className={styles.modalContentButton}>
                <Button title="Подтвердить" onClick={props.onDeleteModul} />
                <Button title="Отмена" onClick={props.onDeleteModulClose} />
            </menu>
        </Modal>
    )
}

export default observer(LoadAssemblyPage);