import type { LatLng } from "leaflet";
import { useState, type Dispatch, type SetStateAction } from "react";
import type { FuelType } from "../../models/FuelType";
import { set } from "zod";

const carKeys = ['ModelName', 'Cost', 'MaxSpeed', 'FuelType', 'Color', 'PlatesNumber'];
const motorcycleKeys = ['ModelName', 'Cost', 'MaxSpeed', 'FuelType', 'Color', 'PlatesNumber'];
const bicycleKeys = ['ModelName', 'Cost', 'IsElectric'];
const scooterKeys = ['ModelName', 'Cost'];

interface CreateSurveyProps {
    type: string | undefined;
    setModelName: Dispatch<SetStateAction<string | undefined>>;
    setCost: Dispatch<SetStateAction<number | undefined>>;
    setMaxSpeed: Dispatch<SetStateAction<number | undefined>>;
    setFuelType: Dispatch<SetStateAction<FuelType | undefined>>;
    setColor: Dispatch<SetStateAction<string | undefined>>;
    setPlatesNumber: Dispatch<SetStateAction<string | undefined>>;
    setIsElectric: Dispatch<SetStateAction<boolean>>;
}

const CreateSurvey: React.FC<CreateSurveyProps> = ({ type, setModelName, setCost, setMaxSpeed, setFuelType, setColor, setPlatesNumber, setIsElectric }) => {
    const handleSet = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;

        switch (name) {
            case 'ModelName': {
                setModelName(value);
                break;
            }
            case 'Cost': {
                setCost(Number(value));
                break;
            }
            case 'MaxSpeed': {
                setMaxSpeed(Number(value));
                break;
            }
            case 'FuelType': {
                setFuelType(value as FuelType);
                break;
            }
            case 'Color': {
                setColor(value);
                break;
            }
            case 'PlatesNumber': {
                setPlatesNumber(value);
                break;
            }
        }
    }
    
    switch (type) {
        case 'Car': {
            return (
            <div>
                {carKeys.map((key) => 
                    <>
                        <label className="block m-2 font-semibold">{key}</label>
                        <input placeholder={`${key}`} name={key} className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {handleSet(e)}}></input>
                    </>)}
            </div>)
        }
        case 'Motorcycle': {
            return (
                <div>
                    {motorcycleKeys.map((key) => 
                        <>
                            <label className="block m-2 font-semibold">{key}</label>
                            <input placeholder={`${key}`} name={key} className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {handleSet(e)}}></input>
                        </>)}
                </div>)
        }
        case 'Bicycle': {
            return (
                <div>
                    {bicycleKeys.map((key) => {
                        if (key === 'IsElectric') {
                            return (<>
                            <label className="block m-2 font-semibold">{key}</label>
                            <select className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setIsElectric(e.target.value === 'true')}}>
                                <option value="true">true</option>
                                <option value="false">false</option>
                            </select>
                        </>)
                        }
                        return (<>
                            <label className="block m-2 font-semibold">{key}</label>
                            <input placeholder={`${key}`} name={key} className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {handleSet(e)}}></input>
                        </>)
                    }
                        )}
                </div>)   
        }
        case 'Scooter': {
            return (
                <div>
                    {scooterKeys.map((key) => 
                        <>
                            <label className="block m-2 font-semibold">{key}</label>
                            <input placeholder={`${key}`} name={key} className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {handleSet(e)}}></input>
                        </>)}
                </div>)
        }
    }

    return (
        <div>
            {carKeys.map((i, key) => 
                <>
                    <label className="block m-2 font-semibold">{key}</label>
                    <input placeholder={`${key}`} className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setDescription(e.target.value)}}></input>
                </>)}
        </div>)
}

export default CreateSurvey;