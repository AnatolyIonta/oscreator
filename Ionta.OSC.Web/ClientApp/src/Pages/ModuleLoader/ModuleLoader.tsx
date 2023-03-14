import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { useHistory } from "react-router-dom";

import styles from "./ModuleLoader.module.css";
import '../../App.css';

import { ApiDomen } from "../../Core/Configure";
import loginStore from "../../Core/LoginStore";
import { InfoModule } from "./ModalInfoModule/ModalInfoModule";
import Button, { ButtonFileLoad } from "../../Controls/Button/Button";
import Strings from "../../Core/LocalizableStrings";
import store from "./ModuleLoaderStore";
import ActivateModule from "../../Controls/EditModule/ActivateModule/ActivateModule";
import { Api } from "../../Core/api";

function LoadAssemblyPage(){
    useEffect(() => {
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
        Api.postAuthWithFile('Assembly/SaveAssembly', file)
        .then(_ => store.load());
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
  
function LoadAssemblyListItem(props:{name:string, id:number, isActive:boolean}) {
    let history = useHistory();
    let className = `${styles.block} row`;

    if (!props.isActive) {
        className = className + " " + styles.blockOff
    }

    let classNameDicorateBox = `${styles.dicorateBox}`;
    if (!props.isActive) {
        classNameDicorateBox = classNameDicorateBox + " " + styles.dicorateBoxOff;
    }

    function onModuleClick() {
        const link = `module/${props.id}`;
        history.push(link);
    }

    return(
        <>
            <div className={className}>
                <div className='row margin-right alignCenter'>
                    <div className={classNameDicorateBox}/>
                    <span className={styles.nameModul}>{props.name}</span>
                </div>
                
                <div className='row margin-right alignCenter justCenter'>
                    <Button onClick={onModuleClick} title={Strings.AsemblyPage.enter}/>
                    <ActivateModule name={props.name} id={props.id.toString()} isActive={props.isActive} isModuleLoaderPage={true}/>
                </div>
            </div>
      </>
    );
}

export default observer(LoadAssemblyPage);