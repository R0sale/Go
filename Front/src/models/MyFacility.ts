export type MyFacility = {
    id: string,
    name: string,
    coordinates: {
        longitude: number,
        latitude: number
    },
    phoneNumber: string,
    email: string,
    description: string,
    websiteURL: string,
    schedule: object
}