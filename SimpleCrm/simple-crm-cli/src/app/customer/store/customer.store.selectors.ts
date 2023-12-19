import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CustomerState, customerStateAdapter } from './customer.store.model';

// Pure functions to read portions of the store.
const {
  selectAll: selectAllCustomers, // select the array of all customers
  selectEntities: selectCustomerEntities, // select the dictionary of customer entities
  selectIds: selectCustomerIds, // select the array of customer ids
  selectTotal: selectCustomerTotal, // select the total customer count
} = customerStateAdapter.getSelectors();

export const customerFeatureKey = 'customer';
const getCustomerFeature =
  createFeatureSelector<CustomerState>(customerFeatureKey);

export const selectCustomers = createSelector(
  getCustomerFeature,
  selectAllCustomers
);
export const selectCustEntities = createSelector(
  getCustomerFeature,
  selectCustomerEntities
);
export const selectCustIds = createSelector(
  getCustomerFeature,
  selectCustomerIds // select the array of customer ids
);
export const selectCustTotal = createSelector(
  getCustomerFeature,
  selectCustomerTotal
);

// To aid in search term persistence when switching from the customer list page and back.
export const selectCriteria = createSelector(
  getCustomerFeature,
  (state: CustomerState) => state.criteria
);

// Discussion at (https://github.com/ngrx/platform/issues/1156)
const getCustomerById = (id: string) => (state: CustomerState) =>
  state.entities[id];
export const selectCustomerById = (id: string) =>
  createSelector(getCustomerFeature, getCustomerById(id));
