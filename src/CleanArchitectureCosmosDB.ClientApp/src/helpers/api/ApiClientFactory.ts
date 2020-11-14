import { Client } from './Resources';

export class ApiClientFactory {
    static GetApiClient(): Client {
        const baseUrl : string = "https://localhost:5001";
        const client : Client = new Client(baseUrl);

        return client;
    }
}