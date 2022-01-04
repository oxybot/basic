import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { apiFetch, apiUrl } from "../api";
import EntityForm from "./EntityForm";

const defaultOnCreate = () => {};

export default function PageNew({ definition, baseApiUrl, texts, onCreate = defaultOnCreate }) {
  const navigate = useNavigate();
  const [entity, setEntity] = useState({});
  const [validated, setValidated] = useState(false);
  texts["form-action"] = "Create";

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setEntity({ ...entity, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    setValidated(true);
    apiFetch(apiUrl(baseApiUrl), {
      method: "POST",
      body: JSON.stringify(entity),
    })
      .then(() => {
        navigate("./..");
        onCreate();
      })
      .catch((err) => {
        console.error(err);
        alert(err);
      });
  };

  return (
    <EntityForm
      definition={definition}
      entity={entity}
      texts={texts}
      handleChange={handleChange}
      handleSubmit={handleSubmit}
      validated={validated}
      container
    />
  );
}
