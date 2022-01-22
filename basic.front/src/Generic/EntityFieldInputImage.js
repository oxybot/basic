export default function EntityFieldInputImage({ field, value, onChange }) {
  function handleRemove() {
    onChange({ target: { name: field.name, value: null } });
  }

  function handleFileChange(e) {
    if (e.target.files.length === 0) {
      // Do nothing: User clicked 'cancel'
      return;
    }

    const file = e.target.files[0];
    const reader = new FileReader();
    reader.onload = function (e) {
      const data = e.target.result.replace(/^data:.+;base64,/, "");
      onChange({ target: { name: field.name, value: { mimeType: file.type, data: data } } });
    };
    reader.readAsDataURL(file);
  }

  return (
    <div className="d-flex">
      {value && <img className="avatar avatar-lg me-2" alt="" src={`data:${value.mimeType};base64,${value.data}`} />}
      {!value && <div className="avatar avatar-lg me-2"></div>}
      <div className="d-flex flex-column align-items-start justify-content-between">
        <input
          type="file"
          className="form-control"
          name={field.name}
          required={field.required}
          accept="image/*"
          onChange={handleFileChange}
        />
        {value && (
          <button type="button" className="btn btn-secondary mt-1" onClick={handleRemove}>
            Remove
          </button>
        )}
      </div>
    </div>
  );
}
