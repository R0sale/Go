import type React from "react";
import { useRef, useState } from "react";


const Images: React.FC = () => {
    const inputRef = useRef(null);
    const [file, setFile] = useState(null);

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setFile(event.target.files?.[0]);
    }

    const onButtonClick = () => {
        inputRef.current.click();
    }
    return (
        <div className="h-screen w-screen">
            <input type="file" ref={inputRef} onChange={handleFileChange} style={{ display: 'none' }}/>
            <button onClick={onButtonClick}>Choose image</button>
        </div>
    )
}

export default Images;