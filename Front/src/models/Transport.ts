interface Transport {
    id: string;
    transportType: string;
    modelName: string;
    cost: number;
    coordinates: {
        latitude: number;
        longitude: number;
    };
}

interface Car extends Transport {
    transportType: 'car';
    maxSpeed: number;
    fuelType: string;
    color: string;
    platesNumber: string;
}

interface Motorcycle extends Transport {
    transportType: 'motorcycle';
    maxSpeed: number;
    fuelType: string;
    color: string;
    platesNumber: string;
}

interface Bicycle extends Transport {
    transportType: 'bicycle';
    isElectric: boolean;
}

interface Scooter extends Transport {
    transportType: 'scooter';
}

type Vehicle = Car | Motorcycle | Bicycle | Scooter;

export type { Vehicle };