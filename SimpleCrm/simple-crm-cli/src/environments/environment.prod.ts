import { CustomerService } from "src/app/customer/customer.service";

export const environment = {
    production: true,
    providers: [
            { provide: CustomerService, useClass: CustomerService }
        ],
};