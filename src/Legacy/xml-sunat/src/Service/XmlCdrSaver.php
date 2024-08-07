<?php

namespace App\Service;

use App\Util\XmlParam;
use Symfony\Component\Filesystem\Filesystem;

class XmlCdrSaver
{
    private Filesystem $filesystem;
    private DataDirProvider $dataDirProvider;

    public function __construct(Filesystem $filesystem, DataDirProvider $dataDirProvider)
    {
        $this->filesystem = $filesystem;
        $this->dataDirProvider = $dataDirProvider;
    }

    public function saveXmlAndCdr(XmlParam $param): void
    {
        $xmlPath = $this->dataDirProvider->getXmlPath($param->companyId, $param->fileName, $param->year, $param->month);
        $cdrPath = $this->dataDirProvider->getCdrPath($param->companyId, $param->fileName, $param->year, $param->month);
        $this->filesystem->mkdir(dirname($xmlPath));
        $this->filesystem->dumpFile($xmlPath, $param->xml);
        $this->filesystem->dumpFile($cdrPath, $param->cdr);
    }
}