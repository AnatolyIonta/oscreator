import { useEffect, useRef } from 'react';
import styles from './Modal.module.css'

interface IModalProps{
    children: any;
    isOpen: boolean;
}

export default function Modal(props: IModalProps) {
    const ref = useRef<HTMLDialogElement>(null);

    useEffect(()=>{
        if(props.isOpen) modalShow();
        else modalClose();
    },[props.isOpen])

    function modalShow(){
        ref.current?.showModal();
    }
    function modalClose(){
        ref.current?.close();
    }
    return(
        <dialog ref={ref} className={styles.modalContent}>
            {props.children}
        </dialog>
    );
}