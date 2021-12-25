import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { apiUrl } from "../api";
import EntityForm from "./EntityForm";

export default function PageNew({ definition, baseApiUrl, texts }) {
  const navigate = useNavigate();
  const [entity, setEntity] = useState({});
  texts["form-action"] = "Create";

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setEntity({ ...entity, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    fetch(apiUrl(baseApiUrl), {
      method: "POST",
      headers: {
        "content-type": "application/json",
        accept: "application/json",
      },
      body: JSON.stringify(entity),
    })
      .then((response) => response.json())
      .then((response) => {
        console.log("ici");
        navigate("./..");
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
      container
    />
  );
}
