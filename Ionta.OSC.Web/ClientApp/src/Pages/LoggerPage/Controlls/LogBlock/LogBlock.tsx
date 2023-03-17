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

    return(
        <div className={classNames} onClick={()=> setOpen(!isOpen)}>
            <div>{props.module}:  {props.message}</div>
            <div hidden={!isOpen}>{props.stackTace}</div>
        </div>
    )
}

export default LogBlock;