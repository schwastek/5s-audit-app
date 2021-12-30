import { instance } from './axios';

function list(urlSearchParams) {
  return instance.get('/audits', { params: urlSearchParams }).then((response) => response.data);
};

function details(id) {
  return instance.get(`/audits/${id}`).then((response) => response.data);
}

function create(audit) {
  return instance.post('/audits', audit).then((response) => response.data);
}

export const Audits = {
  list,
  details,
  create
};
