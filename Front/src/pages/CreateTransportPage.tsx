import React, { useEffect, useState} from "react";
import bgImage from '../assets/worldmap.jpg';
import { MapContainer, Marker, TileLayer, Tooltip} from "react-leaflet";
import getPosition from "../GetPosition";
import type { LatLng, LatLngExpression } from "leaflet";
import { auth } from "../firebase";
import { config } from "../config";
import MapClickHandler from "./components/MapClickHandler";
import * as z from "zod";
import CreateSurvey from "./components/CreateSurvey";
import type { FuelType } from "../models/FuelType";
import { useNavigate } from "react-router-dom";
import { is, th } from "zod/locales";

const transportTypes = ['Car', 'Motorcycle', 'Bicycle', 'Scooter'];

const CreateTransportPage: React.FC = () => {
    const [modelName, setModelName] = useState<string>();
    const [cost, setCost] = useState<number>();
    const [maxSpeed, setMaxSpeed] = useState<number>();
    const [fuelType, setFuelType] = useState<FuelType>();
    const [color, setColor] = useState<string>();
    const [platesNumber, setPlatesNumber] = useState<string>();
    const [isElectric, setIsElectric] = useState<boolean>(false);
    const [center, setCenter] = useState<LatLngExpression>();
    const [loading, setLoading] = useState(false);
    const [coords, setCoords] = useState<LatLng>();
    const [position, setPosition] = useState<LatLngExpression>();
    const [type, setType] = useState<string>();
    const navigate = useNavigate();

    useEffect(() => {
        setLoading(true);
        const getPos = async () => {
            const pos = await getPosition();
            setCenter([pos.coords.latitude, pos.coords.longitude]);
        }

        getPos();

        setLoading(false);
    }, [center]);

    const validate = (transport: object) => {

        const fuleTypeSchema = z.enum(['Gasoline', 'Diesel', 'Electric', 'Hybrid']);
        let Transport;

        switch (type?.toLowerCase()) {
            case 'car': {
                Transport = z.object({
                    modelName: z.string().min(4).max(20),
                    cost: z.number().positive('Uebok').lte(10000),
                    maxSpeed: z.number().positive().lte(300),
                    fuelType: fuleTypeSchema,
                    color: z.string().min(2),
                    platesNumber: z.string().min(5).max(10),
                    coordinates: z.object({
                        latitude: z.number().gte(-90).lte(90),
                        longitude: z.number().gte(-180).lte(180)
                    })
                });
                break;
            }
            case 'motorcycle': {
                Transport = z.object({
                    modelName: z.string().min(4).max(20),
                    cost: z.number().positive().lte(10000),
                    maxSpeed: z.number().positive().lte(300),
                    fuelType: fuleTypeSchema,
                    color: z.string().min(2),
                    platesNumber: z.string().min(5).max(10),
                    coordinates: z.object({
                        latitude: z.number().gte(-90).lte(90),
                        longitude: z.number().gte(-180).lte(180)
                    })
                });
                break;
            }
            case 'bicycle': {
                Transport = z.object({
                    modelName: z.string().min(4).max(20),
                    cost: z.number().positive().lte(10000),
                    isElectric: z.boolean(),
                    coordinates: z.object({
                        latitude: z.number().gte(-90).lte(90),
                        longitude: z.number().gte(-180).lte(180)
                    })
                });
                break;
            }
            case 'scooter': {
                Transport = z.object({
                    modelName: z.string().min(4).max(20),
                    cost: z.number().positive().lte(10000),
                    coordinates: z.object({
                        latitude: z.number().gte(-90).lte(90),
                        longitude: z.number().gte(-180).lte(180)
                    })
                });
                break;
            }
        }

        if (!Transport) {
            throw new Error("Invalid transport type");
        }

        Transport.parse(transport);
    }

    const createTransport = async () => {
        try {
            let obj;
            switch (type?.toLowerCase()) {
                case 'car': {
                    obj  = {
                            modelName: modelName,
                            cost: cost,
                            maxSpeed: maxSpeed,
                            fuelType: fuelType,
                            color: color,
                            platesNumber: platesNumber,
                            coordinates: { 
                                latitude: coords?.lat, 
                                longitude: coords?.lng
                            }
                        };
                    break;
                }
                case 'motorcycle': {
                    obj  = {
                            modelName: modelName,
                            cost: cost,
                            maxSpeed: maxSpeed,
                            fuelType: fuelType,
                            color: color,
                            platesNumber: platesNumber,
                            coordinates: { 
                                latitude: coords?.lat, 
                                longitude: coords?.lng
                            }
                        };
                    break;
                }
                case 'bicycle': {
                    obj  = {
                            modelName: modelName,
                            cost: cost,
                            coordinates: {
                                latitude: coords?.lat,
                                longitude: coords?.lng
                            },
                            isElectric: isElectric
                        };
                    break;
                }
                case 'scooter': {
                    obj  = {
                            modelName: modelName,
                            cost: cost,
                            coordinates: {
                                latitude: coords?.lat,
                                longitude: coords?.lng
                            }
                        };
                    break;
                }
            }

            if (!obj)
                throw new Error("Invalid transport data");

            validate(obj);

            const token = await auth.currentUser?.getIdToken(true);
            let response;

            switch (type?.toLowerCase()) {
                case 'car': {
                    response = await fetch(config.CREATE_CAR_URL, {
                        method: 'POST',
                        headers: {
                            "Content-Type": "application/json",
                            'Authorization': `Bearer ${token}`
                        },
                        body: JSON.stringify(obj)
                    });
                    break;
                }
                case 'motorcycle': {
                    response = await fetch(config.CREATE_MOTORCYCLE_URL, {
                        method: 'POST',
                        headers: {
                            "Content-Type": "application/json",
                            'Authorization': `Bearer ${token}`
                        },
                        body: JSON.stringify(obj)
                    });
                    break;
                }
                case 'bicycle': {
                    response = await fetch(config.CREATE_BICYCLE_URL, {
                        method: 'POST',
                        headers: {
                            "Content-Type": "application/json",
                            'Authorization': `Bearer ${token}`
                        },
                        body: JSON.stringify(obj)
                    });
                    break;
                }
                case 'scooter': {
                    response = await fetch(config.CREATE_SCOOTER_URL, {
                        method: 'POST',
                        headers: {
                            "Content-Type": "application/json",
                            'Authorization': `Bearer ${token}`
                        },
                        body: JSON.stringify(obj)
                    });
                    break;
                }
            }
            

            if (response.ok) {
                alert("Transport created successfully");
                navigate('/');
            } else {
                alert("Something went wrong");
            }   
        } catch (e: any) {
            alert(e.message);
        }
    }

    if (loading) {
        return (<div className="flex h-full w-full">
            <span className="loader m-auto"></span>
        </div>);
    }

    const handleTypeSelection = (e: React.ChangeEvent<HTMLInputElement>) => {
        setType(e.target.value);
    }

    return (
        <div style={{backgroundImage: `url(${bgImage})`, backgroundRepeat: 'space repeat'}} className="w-screen h-screen overflow-x-hidden">
            <div className="w-1/3 bg-white m-auto">
                <div>
                    {transportTypes.map((transport, i) => (<div>
                                <label className="ml-4 mt-2 flex" key={i}>
                                <input type="checkbox" key={i} name={transport} value={transport} checked={type == transport} onChange={e => handleTypeSelection(e)} /> 
                                <span className="ml-2">{transport}</span>
                                </label>
                            </div>))}

                    {type && <CreateSurvey type={type} setModelName={setModelName} setCost={setCost} setColor={setColor} setFuelType={setFuelType} setMaxSpeed={setMaxSpeed} setIsElectric={setIsElectric} setPlatesNumber={setPlatesNumber} />}
                </div>
                {center ? <div>
                    <MapContainer className="h-110 w-full m-2" center={center} zoom={16} scrollWheelZoom={true}>
                        <TileLayer 
                        attribution='&copy; OpenStreetMap contributors'
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                        />
                        <MapClickHandler onClick={(coords: LatLng) => {setPosition(coords); setCoords(coords)}} />
                        {position ? (<>
                         <Marker position={position}>
                            <Tooltip permanent={true}>
                                This is your facility's location
                            </Tooltip>
                         </Marker>
                         </>) : null}
                    </MapContainer>
                </div> : <div>Bez coordinat segodnya</div>}
                <button className="w-30 m-4 h-15 text-xl text-center border-2 border-gray-300" onClick={createTransport}>Submit</button>
            </div>
        </div>);
}

export default CreateTransportPage;