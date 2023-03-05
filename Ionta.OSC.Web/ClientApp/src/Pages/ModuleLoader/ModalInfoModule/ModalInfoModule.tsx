import { useEffect, useState } from 'react';
import JSONViewer from 'react-simple-json-viewer';
import Button from '../../../Controls/Button/Button';
import Modal from '../../../Controls/Modal/Modal';
import { Api } from '../../../Core/api';
import styles from "./ModalInfoModule.module.css";

interface IInfoModuleProps{
    isOpen:boolean;
    onClose: ()=>void;
}

export function InfoModule(props: IInfoModuleProps) {
    const [data, setData] = useState<any>();

    useEffect(()=>{
        loadData();
    },[props.isOpen])

    async function loadData(){
        var resopnse = await Api.postNoBodyAuth("assembly/info");
        if(resopnse.ok){
            setData(await resopnse.json());
        }
    }

    function ModulClose() {
        props.onClose();
    }
    
    return(
        <Modal isOpen={props.isOpen}>
            <JSONViewer data={data} css={{
                'span.rsjv-value': {
                    color: 'purple'
                }
            }}/>
            <div className={styles.modalContentButton}>
                <Button title='Закрыть' onClick={ModulClose}/>
            </div>
        </Modal>
    );
}