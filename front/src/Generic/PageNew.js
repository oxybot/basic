import { useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { addWarning } from "../Alerts/slice";
import { apiFetch } from "../api";
import EntityForm from "./EntityForm";

const defaultOnCreate = () => {};
const defaultExtendedForm = () => null;

export default function PageNew({
  definition,
  baseApiUrl,
  full = false,
  texts,
  onCreate = defaultOnCreate,
  extendedForm = defaultExtendedForm,
}) {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [entity, setEntity] = useState({});
  const [errors, setErrors] = useState({});
  texts["form-action"] = "Create";

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setEntity({ ...entity, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    apiFetch(baseApiUrl, {
      method: "POST",
      body: JSON.stringify(entity),
    })
      .then(() => {
        navigate("./..");
        onCreate();
      })
      .catch((err) => {
        if (err && err.message) {
          dispatch(addWarning("Invalid information", "Please review the values provided."));
        } else if (typeof err === "string") {
          dispatch(addWarning("Invalid information", "Please review the values provided"));
        } else {
          setErrors(err);
        }
      });
  };

  return (
    <EntityForm
      definition={definition}
      entity={entity}
      full={full}
      texts={texts}
      errors={errors}
      handleChange={handleChange}
      handleSubmit={handleSubmit}
    >
      {extendedForm(entity, setEntity, errors)}
    </EntityForm>
  );
}
