import { useEffect, useState } from "react";
import { Api } from "../../../Core/api";
import { useHistory } from "react-router-dom";

import Strings from "../../../Core/LocalizableStrings";
import Button, { ButtonStyles } from "../../Button/Button";
import Modal from "../../Modal/Modal";

import styles from './ActivateModule.module.css';
import store from "../../../Pages/ModuleLoader/ModuleLoaderStore";
import pageStore from "../../../Pages/ModulePage/ModulePageStore";

interface IModuleInfo {
    name: string,
    id: string,
    isActive: boolean,
    isModuleLoaderPage: boolean
}

interface IDeleteWarnningProps {
    isOpen:boolean;
    onDeleteModul: ()=>void;
    onDeleteModulClose: ()=>void;
}

export default function ActivateModule(props: IModuleInfo) {
    const [isOpen, setOpen] = useState<boolean>(false)
    let history = useHistory();

    useEffect(() => {
        choiceLoadPage(props.isModuleLoaderPage);
    },[]);

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
                deleteModulClose();
                
                if (!props.isModuleLoaderPage) {
                    history.push("/");
                } else {
                    store.load();
                }
            })
        });
    }

    function enableModul(){
        const data = {assemblyId: props.id, isActive: !props.isActive};
        let response = Api.postAuth("Assembly/SetActive", data)
        .then(_ => { 
            Api.postAuth("Assembly/ApplayMigration",{}).then(_ => {
                choiceLoadPage(props.isModuleLoaderPage);
            });
        });
    }
    
    return(
        <div>
            <div className='margin-right alignCenter'>
                <Button onClick={enableModul} title={props.isActive ? Strings.AsemblyPage.disableModule : Strings.AsemblyPage.enambleModule}/>
                <Button onClick={deleteModulClick} title={Strings.AsemblyPage.deleteModule} buttonStyle={ButtonStyles.red}/>
            </div>
            <DeleteWarnning isOpen={isOpen} onDeleteModul={deleteModul} onDeleteModulClose={deleteModulClose}/>
        </div>
    )
}

function choiceLoadPage(isModuleLoaderPage: boolean) {
    if (isModuleLoaderPage) {
        store.load();
    } else {
        pageStore.loadModulPageInfo();
    }
}

export function DeleteWarnning(props:IDeleteWarnningProps) {
    return (
        <Modal isOpen={props.isOpen}>
            <span>Вы уверены, что хотите удалить модуль?</span>
            <menu className={styles.modalContentButton}>
                <Button title="Подтвердить" onClick={props.onDeleteModul} />
                <Button title="Отмена" onClick={props.onDeleteModulClose} />
            </menu>
        </Modal>
    )
}