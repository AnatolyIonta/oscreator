import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { useLocation, useParams } from "react-router-dom"; 

import Strings from '../../Core/LocalizableStrings';
import ChangeNameModule from '../../Controls/EditModule/ChangeNameModule/ChangeNameModule'; 

import styles from './ModulePage.module.css';
import ActivateModule from '../../Controls/EditModule/ActivateModule/ActivateModule';
import { ButtonFileLoad } from '../../Controls/Button/Button';
import pageStore from './ModulePageStore';
import { Api } from '../../Core/api';

interface IModuleInfo {
    name: string,
    id: string,
    isActive: boolean
}

function ModulePage() {
    let location = useLocation();
    const params = useParams<{id:string}>();

    useEffect(() => {
        console.log(params);
        
        pageStore.setEntity(Number(params.id))
        pageStore.load();
    },[location.pathname]);

    function refreshModul(e: React.ChangeEvent<HTMLInputElement>) {
        const file = e.target!.files![0];
        const url = location.pathname.split('/');
        const id = url[url.length - 1];

        if (file) Api.postAuthWithFile(`Assembly/refreshModul/${id}`, file)
            .then(_ => pageStore.load());
    }

    if(pageStore.entity == null) return <span>Loading...</span>

    return(
        <div>
            <div className={styles.content}>
                <ModuleInfo name={pageStore.entity.name} id={String(pageStore.entity.id)} isActive={pageStore.entity.isActive}/>
            </div>
            <footer className={styles.footer}>
                <ButtonFileLoad title={Strings.ModulePage.refresh} onChange={refreshModul}/>
                <ActivateModule name={pageStore.entity.name} id={String(pageStore.entity.id)} isActive={pageStore.entity.isActive} isModuleLoaderPage={false}/>
            </footer>
        </div>
    )
}

function ModuleInfo(props: IModuleInfo){
    return(
        <div>
            <div>
                <h1>
                    {Strings.ModulePage.moduleTitle} {props.name}  
                    {props.isActive ? <>{Strings.ModulePage.isActive}</> : <>{Strings.ModulePage.isNotActive}</>}
                </h1>
            </div>
            <div>
                <ChangeNameModule id={props.id}/>
            </div>
        </div>
    )
}

export default observer(ModulePage);