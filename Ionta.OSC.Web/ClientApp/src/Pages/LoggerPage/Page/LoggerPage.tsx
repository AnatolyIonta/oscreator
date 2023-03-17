import '../../../App.css';
import Button from '../../../Controls/Button/Button';
import HeaderPage from '../../../Controls/HeaderPage/HeaderPage';
import LogBlock from '../Controlls/LogBlock/LogBlock';
import ILogData from '../Core/LogData';
import LogType from '../Core/LogType';

const testData : ILogData[] = [
    {
        module: "test",
        message:"Hello world",
        type: 0,
    },
    {
        module: "test3",
        message:"Hello world",
        type: 1,
    },
    {
        module: "test3",
        message:"Hello worldas sa dada d",
        type: 1,
        stackTace: "1234adas das das d asdas das das da s das da s"
    },
    {
        module: "test",
        message:"Hello world",
        type: 0,
    },
    {
        module: "test",
        message:"Hello world",
        type: 1,
    },
    {
        module: "test",
        message:"Hello world",
        type: 1,
    },
    {
        module: "test",
        message:"Hello world",
        type: 1,
    },
    {
        module: "test",
        message:"Hello world",
        type: 1,
    },
    {
        module: "test",
        message:"Hello world",
        type: 1,
    },
    {
        module: "test",
        message:"Hello world",
        type: 0,
    },
]

function LoggerPage(){
    return(
        <div>
            <HeaderPage title='Логи'/>
            <LogList data={testData}/>
        </div>
    )
}

function LogList(props:{data:ILogData[]}){
    const data = props.data;
    return(
        <div className='column gap'>
            {data.map((d,i) => <LogBlock key={d.module+i} module={d.module} message={d.message} type={d.type} stackTace={d.stackTace}/>)}
        </div>
    )
}

export default LoggerPage;