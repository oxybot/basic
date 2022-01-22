import { useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { addWarning } from "../Alerts/slice";
import { apiFetch, apiUrl } from "../api";
import EntityForm from "./EntityForm";

const defaultOnCreate = () => {};

export default function PageNew({ definition, baseApiUrl, texts, onCreate = defaultOnCreate }) {
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
    apiFetch(apiUrl(baseApiUrl), {
      method: "POST",
      body: JSON.stringify(entity),
    })
      .then(() => {
        navigate("./..");
        onCreate();
      })
      .catch((err) => {
        if (typeof err === "string") {
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
      texts={texts}
      errors={errors}
      handleChange={handleChange}
      handleSubmit={handleSubmit}
      container
    />
  );
}
