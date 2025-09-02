interface Transport {
    id: string;
    type: string;
    modelName: string;
    cost: number;
    coordinates: {
        latitude: number;
        longitude: number;
    };
}

interface Car extends Transport {
    type: 'car';
    maxSpeed: number;
    fuelType: string;
    color: string;
    platesNumber: string;
}

interface Motorcycle extends Transport {
    type: 'motorcycle';
    maxSpeed: number;
    fuelType: string;
    color: string;
    platesNumber: string;
}

interface Bicycle extends Transport {
    type: 'bicycle';
    isElectric: boolean;
}

interface Scooter extends Transport {
    type: 'scooter';
}

type Vehicle = Car | Motorcycle | Bicycle | Scooter;

export type { Vehicle };