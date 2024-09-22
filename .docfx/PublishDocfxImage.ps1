$version = minver -i -t v -v w
docker tag newtonsoft-json-docfx:$version jcr.codebelt.net/geekle/newtonsoft-json-docfx:$version
docker push jcr.codebelt.net/geekle/newtonsoft-json-docfx:$version
