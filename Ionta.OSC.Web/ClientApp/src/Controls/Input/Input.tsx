import React from 'react';
import Styles from './Input.module.css';

interface IInputProps {
    onChangeValue:(e:string)=>void,
    value:string,
    name?:string,
    id?:string,
    inputStyles?: InputStyles
}

export enum InputStyles {
    classic,
    moderate
}

function Input(props: IInputProps) {
    function onChange(event: React.ChangeEvent<HTMLInputElement>){
        props.onChangeValue(event.target.value);
    }
    
    return(
        <input className={Styles.classic} onChange={onChange} value={props.value} name={props.name} id={props.id}/>
    )
}
export default Input;