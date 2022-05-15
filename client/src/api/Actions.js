import { instance } from './axios';

function create(action) {
  return instance.post('/actions', action).then((response) => response.data);
}

function remove(actionId) {
  return instance.delete(`/actions/${actionId}`).then((response) => response.data);
}

function update(actionId, action) {
  return instance.put(`/actions/${actionId}`, action).then((response) => response.data);
}

export const Actions = {
  create,
  remove,
  update
};
