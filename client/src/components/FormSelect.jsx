import React, { useState } from 'react';

function FormSelect({ onChange }) {
  const [selectedArea, selectArea] = useState('');

  function handleChange(event) {
    const value = event.target.value;
    selectArea(value);
    if (onChange) onChange(value);
  }

  return (
    <>
      <label htmlFor="area-select" className="mb-2">Select area:</label>
      <select value={selectedArea} onChange={handleChange} id="area-select" className="form-select mb-4">
        <option value="assembly">Assembly</option>
        <option value="packing">Packing</option>
        <option value="shipping">Shipping</option>
        <option value="storage">Storage</option>
        <option value="testing">Testing</option>
      </select>
    </>
  );
}

export { FormSelect };
