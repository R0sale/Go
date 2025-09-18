import type { Node } from "./Node";

export type Route = {
    name: string;
    city: string;
    description: string;
    nodes: Node[];
}