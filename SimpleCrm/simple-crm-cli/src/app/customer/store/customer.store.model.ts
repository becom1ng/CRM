import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { Customer } from '../customer.model';

/** Defines the state shape/model interface(s) and the initial state (const) value. */
export interface CustomerState extends EntityState<Customer> {
  // additional entity state properties
  criteria: customerSearchCriteria;
  searchStatus: string;
  addCustomerStatus: string;
  updateCustomerStatus: string;
}

export const customerStateAdapter: EntityAdapter<Customer> =
  createEntityAdapter<Customer>({
    selectId: (item: Customer) => item.customerId, // <-- defines the key property
  });

export const initialCustomerState: CustomerState =
  customerStateAdapter.getInitialState({
    criteria: { term: '' },
    searchStatus: '',
    addCustomerStatus: '',
    updateCustomerStatus: '',
  });

// ? TODO: See CustomerListParameters from SimpleCRM for more params
export interface customerSearchCriteria {
  term: string;
}
