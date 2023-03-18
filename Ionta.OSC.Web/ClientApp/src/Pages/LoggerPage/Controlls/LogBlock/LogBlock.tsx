import { useState } from 'react';
import '../../../../App.css';
import LogType from '../../Core/LogType';
import styles from "./LogBlock.module.css";

interface ILogBlock{
    type:LogType,
    module: string,
    message: string,
    stackTace?:string,
}

function LogBlock(props:ILogBlock){
    const [isOpen, setOpen] = useState<boolean>(false);
    let classNames = `${styles.block} column gap `;


    if(props.type == LogType.Error){
        classNames += "errorPanel";
    }
    else if(props.type == LogType.Seccses){
        classNames += "successPanel";
    }
    else if(props.type == LogType.Warrning){
        classNames += "warrningPanel";
    }

    return(
        <div className={classNames} onClick={()=> setOpen(!isOpen)}>
            <div>{props.module}:  {props.message}</div>
            <div hidden={!isOpen} className={styles.stack}><pre>{props.stackTace}</pre></div>
        </div>
    )
}

export default LogBlock;