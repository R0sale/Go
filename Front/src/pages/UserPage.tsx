import { auth } from "../firebase";
import guestImage from "../assets/guest.png";
import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import type { MyFacility } from "../models/MyFacility";
import { config } from "../config";
import type { TokenResult } from "../models/TokenResult";



const UserPage: React.FC = () => {
    const navigate = useNavigate();
    const [tokenResult, setTokenResult] = useState<TokenResult>({
    userName: '',
    firstName: '',
    lastName: '',
    email: '',
    roles: [],
});
    const [loading, setLoading] = useState(false);
    const [facilities, setFacilities] = useState<MyFacility[]>([]);
    const [userImage, setUserImage] = useState<string>(guestImage);
    const inputRef = useRef(null);

    useEffect(() => {
        setLoading(true);

        const unsubscribe = auth.onAuthStateChanged(async (user) => {
            if (user) {
                const token = await user.getIdTokenResult();
                console.log(token.claims.roles);
                setTokenResult(token.claims);
            } else {
                setTokenResult({
                    userName: '',
                    firstName: '',
                    lastName: '',
                    email: '',
                    roles: [],
                });
            }

            setLoading(false);
        });

        

        return () => unsubscribe();
    }, []);

    useEffect(() => {
        const unsubscribe = auth.onAuthStateChanged(async (user) => {
            if (user) {
                const response = await fetch(config.AZURE_USERS_URL + user.uid);

                if (response.status == 200) {
                    setUserImage(config.AZURE_USERS_URL + user.uid);
                }
            } 
        });

        return () => unsubscribe();
    }, []);

    const getFacilities = async () => {
        if (tokenResult.roles.map((role: string) => role == 'Admin'))
        {
            const token = await auth.currentUser.getIdToken();

            try {
                const result = await fetch(config.USERS_FACILITIES_URL, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });
            
                const response = await result.json();

                setFacilities(response);

            } catch (er) {
                if (er instanceof Error)
                {
                    alert(`Error: ${er.message}`);
                } else
                {
                    alert(`Some undefined error occured.`);
                }
            }
            
        }
    }


    if (loading) {
        return (<div className="flex h-screen w-screen">
            <span className="loader m-auto"></span>
        </div>);
    }

    const data = [
        {label: "Username", data: `${tokenResult.userName}`},
        {label: "Full name", data: `${tokenResult.firstName} ${tokenResult.lastName}`},
        {label: "Email", data: `${tokenResult.email}`},
        {label: "Roles", data: `${tokenResult.roles}`}
    ];

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files?.[0]) {
            const newFile = event.target.files[0];

            handleImageUpload(newFile);
        }
    }

    const downloadUserInfo = async () => {
        try {
            const token = await auth.currentUser.getIdToken(true);

            const response = await fetch(config.DOWNLOAD_USER_INFO_URL, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error('Failed to download user info');
            }

            const blob = await response.blob();

            if (!blob) {
                throw new Error('No data received');
            }

            const url = window.URL.createObjectURL(blob);

            const link = document.createElement('a');
            link.href = url;
            link.download = "YourInfo.pdf";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        } catch (er) {
            if (er instanceof Error) {
                alert(`Error: ${er.message}`);
            } else {
                alert(`Some undefined error occured.`);
            }
        }
    }

    const handleImageClick = () => {
        inputRef.current.click();
    }

    const handleImageUpload = async (newFile: File) => {
        if (newFile == null) {
            alert("No file selected");
            return;
        }

        const formData = new FormData();
        formData.append("file", newFile);

        if (auth.currentUser == null) {
            alert("User not authenticated");
            return;
        }

        const response = await fetch(config.CHANGE_USERS_IMAGE_URL, {
            method: 'POST',
            headers: {
                "Authorization": `Bearer ${await auth.currentUser.getIdToken()}`
            },
            body: formData
        });

        if (response.ok) {
            setUserImage(config.AZURE_USERS_URL + auth.currentUser.uid);
        }
    }

    return (
        <div className="h-screen flex pinned-left bg-gray-100 overflow-y-hidden">
            <div className="mt-4 mb-4 ml-6 mr-6 w-468 bg-white ">
                <div className="h-1/2 bg-white">
                    <div className="p-20 flex">
                        <input type="file" style={{ display: 'none' }} ref={inputRef} onChange={handleFileChange}></input>
                        <div className="w-200 ">
                            <img className="rounded-full w-60 h-60 block cursor-pointer" src={userImage} onClick={handleImageClick}></img>
                            <div className="ml-4 mt-20 font-semibold h-10 w-60 text-2xl flex justify-between">
                                <p>Account</p>
                            </div>
                        </div>
                        <div className="ml-0">
                            <p className="text-6xl font-bold">{tokenResult.firstName} {tokenResult.lastName}</p>
                            <p className="text-2xl text-blue-600 underline mt-4 decoration-2">{tokenResult.email}</p>
                            <button className="w-48 h-15 text-center items-center text-xl mt-10 flex border-2 border-gray-300" onClick={async () => {await downloadUserInfo()}}>Download my information</button>
                        </div>
                        <div className="block ml-120"> 
                            <button className="w-48 h-15 text-center items-center text-xl flex border-2 border-gray-300" onClick={() => {navigate('/')}}>To Main Page</button>
                            {tokenResult.roles.includes('Admin') && <button className="w-48 h-15 text-center items-center text-xl mt-10 flex border-2 border-gray-300" onClick={() => {navigate('/routes')}}>To Routes Page</button>}
                            {tokenResult.roles.includes('Admin') && <button className="w-48 h-15 text-center items-center text-xl  mt-10 flex border-2 border-gray-300" onClick={() => {navigate('/userPage/transport')}}>To Transport Page</button>}
                            {tokenResult.roles.includes('Admin') && <button className="w-48 h-15 text-center items-center text-xl flex border-2 mt-10 border-gray-300" onClick={() => {navigate('/userPage/facilityPage')}}>Create New Facility</button>}
                        </div>
                    </div>
                    
                    
                </div>
                <div className="h-1 bg-gray-100 mt-5 m-0 p-0 w-full"></div>
                <div className="flex h-109">
                    <div className="w-2/3">
                        {data.map((row, i) => {
                            return (<div className="ml-4 mt-4 text-xl justify-between flex" key={i}>
                            <p className="p-2">{row.label}</p>
                            <p className="mr-10 w-140 p-2  rounded-2xl border-1 bg-gray-100">{row.data}</p>
                        </div>);
                        })}
                    </div>
                    <div className="w-1/3 m-2 h-100">
                        <div className="bg-gray-100 rounded-2xl border-2 border-black h-full">
                            <div className="flex justify-between">
                                <p className="m-2 mt-4 text-2xl w-1/3">Your facilities:</p>
                                <button className="w-35 h-12 text-center items-center text-l m-2 border-2 border-gray-300" onClick={getFacilities}>Get Facilities</button>
                            </div>
                            <p className="h-1 bg-white m-0 p-0 w-full"></p>
                            <div className="overflow-y-scroll h-80">
                                {facilities.map((facility, i) => {
                                    return (
                                        <div key={i} className="p-2 w-full h-15 leading-10.5 font-medium cursor-pointer hover:bg-gray-200" onClick={() => {navigate(`/userPage/facilityPage/${facility.id}`)}}>
                                            {facility.name}: {facility.email}
                                        </div>
                                    );
                                })}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default UserPage;