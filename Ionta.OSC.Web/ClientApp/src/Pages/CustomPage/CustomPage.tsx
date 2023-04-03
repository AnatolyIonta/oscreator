import { log } from "console";
import { useEffect, useRef, useState } from "react";
import ReactDOM from 'react-dom/client';
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
        
        let script = ref.current?.getElementsByTagName("script")[0];
		let templates = ref.current?.getElementsByTagName("template");
		
        console.log(templates);
        

		for(let i = 0; i < (templates?.length ?? 0); i++){
            let item = templates!.item(i)!.content;
            let scriptTemplate = item.querySelector("script");
            if(scriptTemplate){
                let newScript = updateJs(scriptTemplate);
                item.removeChild(scriptTemplate);
                item.appendChild(newScript);
            }
        }
		
        if(script){
            var newScript = updateJs(script);
            ref.current?.removeChild(script);
            document.body.appendChild(newScript);
        }
    },[html])

    function updateJs(script:HTMLScriptElement){
        let script1 = document.createElement('script');
        script1.setAttribute("type", script.type);
        script1.text = script.text;
        return script1;
    }

    return(
        <>
            <div ref={ref} id={"workspace"} style={{height:"100%"}}>

            </div>
        </>
    )
}

export default CustomPage;