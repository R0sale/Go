import { useEffect, useState } from "react";
import { auth } from "../firebase";
import { config } from "../config";
import type { Vehicle } from "../models/Transport";
import TransportMap from "./components/TransportMap";
import type { KeyValueObject } from "../models/KeyValueObject";
import { useNavigate } from "react-router-dom";

const TransportPage: React.FC = () => {
    const [transport, setTransport] = useState<Vehicle[]>();
    const [token, setToken] = useState<string>();
    const [selectedTransport, setSelectedTransport] = useState<Vehicle>();
    const [information, setInformation] = useState<KeyValueObject[]>([]);
    const navigate = useNavigate();

    const transportInformation = async (vehicle: Vehicle) => {
        switch (vehicle.transportType.toLowerCase()) {
            case 'car': {
                const response = await fetch(config.GET_SELECTED_CAR_URL + vehicle.id);

                const result = await response.json();

                setInformation(result);
                break;
            }
            case 'motorcycle': {
                const response = await fetch(config.GET_SELECTED_MOTORCYCLE_URL + vehicle.id);

                const result = await response.json();

                setInformation(result);
                break;
            }
            case 'bicycle': {
                const response = await fetch(config.GET_SELECTED_BICYCLE_URL + vehicle.id);

                const result = await response.json();

                setInformation(result);
                break;
            }
            case 'scooter': {
                const response = await fetch(config.GET_SELECTED_SCOOTER_URL + vehicle.id);

                const result = await response.json();

                setInformation(result);
                break;
            }
        }
    }

    useEffect(() => {
        const unsubscribe = auth.onAuthStateChanged(async (user) => {
            if (user) {
                const token = await user.getIdToken(true);
                setToken(token);

                const response = await fetch(config.GET_USERS_TRANSPORT, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });

                const result = await response.json();

                setTransport(result);
            }
        });

        return () => unsubscribe();
    }, []);

    const updateTransport = async () => {
        const response = await fetch(config.GET_USERS_TRANSPORT, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });

        const result = await response.json();

        setTransport(result);
    }

    return (
        <div className="h-screen flex pinned-left bg-gray-100 overflow-y-hidden">
            <div className="mt-4 mb-4 ml-6 mr-6 w-468 bg-white flex">
                <div className="bg-gray-100 border-1 rounded-2xl ml-2 w-1/2 border-black h-full">
                    <div className="flex justify-between">
                        <p className="m-2 mt-4 text-2xl w-1/3">Your transport:</p>
                        <button className="w-50 h-12 text-center items-center text-l m-2 border-2 border-gray-300" onClick={() => navigate('/userPage/createtransport')}>Create Transport</button>
                        <button className="w-35 h-12 text-center items-center text-l m-2 border-2 border-gray-300" onClick={async () => {await updateTransport()}}>Update</button>
                    </div>
                    <p className="h-1 bg-white m-0 p-0 w-full"></p>
                    <div className="overflow-y-scroll h-201">
                        {transport && transport.map((t, i) => {
                            return (
                                <div key={i} className="p-2 w-full h-20 leading-14 font-medium cursor-pointer text-4xl hover:bg-gray-200" onClick={() => {setSelectedTransport(t); transportInformation(t);}}>
                                    {t.transportType}: {t.modelName}
                                </div>
                            );
                        })}
                    </div>
                </div>
                <div className="w-1/2 h-full">
                        <TransportMap coords={selectedTransport?.coordinates ? [selectedTransport.coordinates.latitude, selectedTransport.coordinates.latitude] : [45.2671, 19.8335]} />
                    <div className="w-full h-102">
                        {(selectedTransport && information.length) && information?.map((row, i) => {
                            if (row.label == 'Coordinates' || row.label === 'Id' || row.label === 'UserId')
                                return;
                            
                            return (<div className="ml-4 mt-2 text-xl justify-between flex" key={i}>
                                        <p className="p-2">{row.label}</p>
                                        <p className="mr-10 w-140 p-2  rounded-2xl border-1 bg-gray-100">{row.value}</p>
                                    </div>)
                        })}
                    </div>
                </div>
            </div>
        </div>
    )
}

export default TransportPage;