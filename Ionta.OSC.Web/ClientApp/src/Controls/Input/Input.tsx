import React from 'react';

import Styles from './Input.module.css';

function Input(props:{onChangeValue:(e:string)=>void, value:string, name?:string, id?:string}){
    
    function onChange(event: React.ChangeEvent<HTMLInputElement>){
        props.onChangeValue(event.target.value);
    }

    return(
        <input className={Styles.classic} onChange={onChange} value={props.value} name={props.name} id={props.id}/>
    )
}

export default Input;