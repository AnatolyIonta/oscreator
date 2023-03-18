import { useEffect } from 'react';
import '../../../App.css';
import Button from '../../../Controls/Button/Button';
import HeaderPage from '../../../Controls/HeaderPage/HeaderPage';
import LogBlock from '../Controlls/LogBlock/LogBlock';
import ILogData from '../Core/LogData';
import LogType from '../Core/LogType';
import store from './LoggerPageStore';

function LoggerPage(){
    useEffect(()=>{
        store.load();
    },[])
    return(
        <div>
            <HeaderPage title='Логи'/>
            <LogList data={store.data}/>
        </div>
    )
}

function LogList(props:{data:ILogData[] | null}){
    const data = props.data;

    if(data == null) return <span>Loading...</span>

    return(
        <div className='column gap'>
            {data.map((d,i) => <LogBlock key={d.module+i} module={d.module} message={d.message} type={d.type} stackTace={d.stackTace}/>)}
        </div>
    )
}

export default LoggerPage;