import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import '../../../App.css';
import HeaderPage from '../../../Controls/HeaderPage/HeaderPage';
import LogBlock from '../Controlls/LogBlock/LogBlock';
import ILogData from '../Core/LogData';
import store from '../Core/LoggerPageStore';

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
            {data.map((d,i) => <LogBlock key={"Logs"+i} module={new Date(d.date).toLocaleString()} message={d.message} type={d.type} stackTace={d.stackTace}/>)}
        </div>
    )
}

export default observer(LoggerPage);