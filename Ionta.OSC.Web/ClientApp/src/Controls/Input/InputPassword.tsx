import React from 'react';

import Styles from './Input.module.css';

function InputPassword(props:{onChangeValue:(e:string)=>void, value:string, name?:string, id?:string}){
    
    function onChange(event: React.ChangeEvent<HTMLInputElement>){
        props.onChangeValue(event.target.value);
    }

    return(
        <input type={"password"} className={Styles.classic} onChange={onChange} value={props.value} name={props.name} id={props.id}/>
    )
}

export default InputPassword;