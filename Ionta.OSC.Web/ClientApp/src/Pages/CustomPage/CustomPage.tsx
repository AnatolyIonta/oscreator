import { useEffect, useRef, useState } from "react";
import { useLocation } from "react-router-dom"
import Button, { ButtonStyles } from "../../Controls/Button/Button";
import { Api } from "../../Core/api";

function CustomPage(){
    const location = useLocation();
    const [html, setHtml] = useState<string>("");
    const ref = useRef<HTMLDivElement>(null);

    useEffect(()=>{
        Api.postAuth("workspace/getpage",{url:location.pathname}).then(async (response)=>{
            let json = await response.json();
            setHtml(json.html);
        })
    },[location.pathname])

    useEffect(()=>{
        ref.current!.innerHTML = html;

        var script = ref.current?.getElementsByTagName("script")[0];
        if(script){
            updateJs(script);
            ref.current?.removeChild(script);
        }
    },[html])

    function updateJs(script:HTMLScriptElement){
        var docScripts = document.body.getElementsByTagName("script");
        
        for(let i = 0; i < docScripts.length; i++){
            try{
                document.body.removeChild(docScripts[i]);
            }
            catch{

            }
        }

        let script1 = document.createElement('script');
        script1.text = script.text;
        document.body.appendChild(script1);
    }

    return(
        <>
            <div ref={ref}>

            </div>
        </>
    )
}

export default CustomPage;