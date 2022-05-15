import React, { useState } from 'react';

const descriptionMaxLength = 280;

function AuditActionsNewForm({ onSubmitAction }) {
  const [editMode, setEditMode] = useState(false);
  const [description, setDescription] = useState('');

  function handleSubmit(event) {
    event.preventDefault();
    handleCloseForm();
    setDescription('');
    onSubmitAction(description);
  }

  function handleChange(event) {
    const { value } = event.target;
    const description = value.slice(0, descriptionMaxLength);
    setDescription(description);
  }

  function handleOpenForm() {
    setEditMode(true);
  }

  function handleCloseForm() {
    setEditMode(false);
  }

  return (
    <>
      <div className="mb-3">
        <button type="button" className="btn btn-primary" onClick={handleOpenForm}>Add action</button>
      </div>

      {editMode &&
        <form onSubmit={handleSubmit}>
          <label htmlFor="auditActionDescription" className="form-label">Action description ({description.length}/{descriptionMaxLength}):</label>
          <textarea className="form-control" id="auditActionDescription" rows="5" value={description} onChange={handleChange} maxLength={descriptionMaxLength}></textarea>
          <div className="my-2 mb-4">
            <button type="submit" className="btn btn-primary me-1">Save</button>
            <button type="button" className="btn btn-danger" onClick={handleCloseForm}>Cancel</button>
          </div>
        </form>
      }
    </>
  );
}

export { AuditActionsNewForm };
