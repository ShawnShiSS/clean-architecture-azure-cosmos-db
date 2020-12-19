import { ToDoItemClient } from './Resources';

export class ApiClientFactory {
    static GetToDoItemClient(): ToDoItemClient {
        const baseUrl : string = "https://localhost:5001";
        const client : ToDoItemClient = new ToDoItemClient(baseUrl);

        return client;
    }
}